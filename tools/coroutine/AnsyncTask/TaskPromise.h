#pragma once

#include <functional>
#include <coroutine>
#include <mutex>
#include <list>
#include <optional>

#include "Result.h"
#include "TaskAwaiter.h"

template <typename ResultType>
class Task;

template <typename ResultType>
struct TaskPromise
{
    // 协程立即执行
    std::suspend_never initial_suspend() { return {}; }

    // 执行结束后挂起，等待外部销毁
    std::suspend_always final_suspend() noexcept { return {}; }

    // 构造协程的返回对象 Task
    Task<ResultType> get_return_object()
    {
        return Task{std::coroutine_handle<TaskPromise>::from_promise(*this)};
    }

    void unhandled_exception()
    {
        std::lock_guard lock(completion_lock);
        result = Result<ResultType>(std::current_exception());
        completion.notify_all();
        notify_callbacks();
    }

    template <typename _ResultType>
    TaskAwaiter<_ResultType> await_transform(Task<_ResultType> &&task)
    {
        return TaskAwaiter<_ResultType>(std::move(task));
    }

    void return_value(ResultType value)
    {
        std::lock_guard lock(completion_lock);
        result = Result<ResultType>(std::move(value));
        completion.notify_all();
        notify_callbacks();
    }

    ResultType get_result()
    {
        std::unique_lock lock(completion_lock);
        if (!result.has_value())
        {
            completion.wait(lock);
        }
        return result->get_or_throw();
    }

    void on_completed(std::function<void(Result<ResultType>)> &&func)
    {
        std::unique_lock lock(completion_lock);
        if (result.has_value())
        {
            auto value = result.value();
            lock.unlock();
            func(value);
        }
        else
        {
            completion_callbacks.push_back(func);
        }
    }

private:
    std::optional<Result<ResultType>> result;
    std::mutex completion_lock;
    std::condition_variable completion;
    std::list<std::function<void(Result<ResultType>)>> completion_callbacks;

    void notify_callbacks()
    {
        auto value = result.value();
        for (auto &callback : completion_callbacks)
        {
            callback(value);
        }
        completion_callbacks.clear();
    }
};

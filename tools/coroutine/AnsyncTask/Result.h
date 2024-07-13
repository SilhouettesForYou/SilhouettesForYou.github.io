#include <exception>

template <typename T>
struct Result
{
    explicit Result() = default;

    // 当 Task 正常返回时用结果初始化 Result
    explicit Result(T &&value) : _value(value) {}
    // 当 Task 抛异常是用异常初始化 Result
    explicit Result(std::exception_ptr &&exception_ptr) : _exception_ptr(exception_ptr) {}

    T get_or_throw()
    {
        if (_exception_ptr)
        {
            std::rethrow_exception(_exception_ptr);
        }
        return _value;
    }

private:
    T _value{};
    std::exception_ptr _exception_ptr;
};
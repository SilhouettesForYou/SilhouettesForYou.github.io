#include<print>
#include<iostream>
#include<variant>

struct A
{
    void test() { std::println("A"); }
};

struct B
{
    void test() { std::println("B"); }
};

int main()
{
    std::variant<A, B> v;
    std::variant<int, float> v2;

    std::visit([](auto &x) { x.test(); }, v);
    std::visit([](auto &x) {
        if constexpr (std::is_same_v<decltype(x), int>)
        {
            std::println("int {}", x);
        }
        else if constexpr (std::is_same_v<decltype(x), float>)
        {
            std::println("float {}", x);
        }
    }, v2);
    return 0;
}
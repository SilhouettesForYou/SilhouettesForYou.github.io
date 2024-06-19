#include <algorithm>
#include <optional>
#include <string>
#include <iostream>

std::optional<std::size_t> FindInStr(const std::string &str, auto pred)
{
    auto it = std::ranges::find_if(str, pred);
    if (it == str.end())
        return std::nullopt;
    return it - str.begin();
}

int main()
{
    std::string s{ "123" };
    auto pos = FindInStr(s, [](char c)
    {
        return c >= '0' && c <= '9' && ((c - '0') % 2 == 0);
    });
    auto c = pos.transform([&s](std::size_t idx)
    {
        std::cout << "The first occurrence is " << s[idx];
        return s[idx];
    }).or_else([]()
    {
        std::cout << "No occurrence found.";
        return std::optional{ '?' };
    });
    std::cout << "The final character is " << *c;
    return 0;
}
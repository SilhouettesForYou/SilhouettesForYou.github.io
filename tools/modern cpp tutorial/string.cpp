#include <string>
#include <print>

int main()
{
    std::string str{ "Test, Test" };
    str.resize_and_overwrite(20, [](char* ptr, std::size_t len)
    {
        for (std::size_t i = 0; i < len - 1; i++)
            ptr[i] = 'a' + i;
        return len - 1;
    });
    std::println("{}", str);
    return 0;
}
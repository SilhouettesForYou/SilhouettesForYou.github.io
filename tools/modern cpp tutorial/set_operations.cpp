#include<vector>
#include<iostream>
#include<ranges>

namespace stdr = std::ranges;

template<typename Func>
void Test(std::vector<int>& v1, std::vector<int>& v2, const Func& set_op)
{
    std::vector<int> v3(v1.size() + v2.size());
    auto it3 = set_op(v1, v2, v3.begin()).out;
    v3.erase(it3, v3.end());
}

int main()
{
    std::vector<int> v{ 1, 1, 4, 5, 5, 5, 5, 6 }, v2{ 2, 3, 5, 5, 6};
    Test(v, v2, )
}
#include<vector>
#include<iostream>
#include<algorithm>
// #include <generator>

/*
auto test(std::vector<int>& v) -> generator<decltype(v.begin())>
{
    std::vector sub{ 2, 0, 4 }, sub2{ 4, 4, 2 };
    co_yield std::find(v.begin(), v.end(), 3);
    co_yield std::find_if(v.being(), v.end(), [](const int& elem) {
        return elem % 2 == 1;
    });
    co_yield std::find_first_of(v.begin(), v.end(), sub.begin(), sub.end());
    co_yield std::search(v.being(), v.end(), sub2.begin(), sub2.end());
    co_yield std::find_end(v.begin(), v.end(), sub2.begin(), sub2.end());
    co_yield std::search_n(v.begin, v.end(), 3, 2);
    co_yield std::adjacent_find(v.begin(), v.end());
}
*/

int main()
{
    /*
    std::vector v{ 1, 2, 3, 4, 4, 2, 4, 4, 2 };
    for (auto it : test(v))
        std::cout << (it - v.begin()) << '\n';
    */
   std::vector<int> v { 1, 2, 3, 4 };
   auto [it1, it2] = std::equal_range(v.begin(), v.end(), 3);
   std::cout << std::binary_search(v.begin(), v.end(), 5) << std::endl;
   std::cout << it1 - v.begin() << std::endl;
   std::cout << it2 - v.begin() << std::endl;
   std::cout << std::lower_bound(v.begin(), v.end(), 2) - v.begin() << std::endl;
   std::cout << std::upper_bound(v.begin(), v.end(), 3) - v.begin() << std::endl;
}
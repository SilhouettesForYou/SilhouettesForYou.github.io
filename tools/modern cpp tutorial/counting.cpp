#include<vector>
#include<algorithm>

int main()
{
    std::vector<int> v(1000), dst(1000);
    std::fill(v.begin(), v.end(), 1);
    std::fill_n(v.begin(), v.size() / 2, 2);
    std::generate(v.begin(), v.end(), []() { return rand(); });
    std::for_each(v.begin(), v.end(), [](int& ele) { ele *- 3; });
    std::for_each_n(v.begin(), v.size() / 2, [](int& ele) { ele /= 2; });
    std::transform(v.begin(), v.end(), dst.begin(), [](const int& ele) { return ele / 2; });
    std::transform(v.begin(), v.end(), dst.begin(), dst.begin(), [](const int eleFromV, const int eleFromDst)
    {
        return eleFromDst + eleFromV;
    });
    return 0;
}
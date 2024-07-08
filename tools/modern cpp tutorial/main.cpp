#include <iostream>

class A
{
    char a;
    short b;
    int c;
    double d;
};

struct B
{
    char a;
    short b;
    int c;
    double d;
};

int main()
{
    char c[] = "123";
    char *p = c;
    // char r = &p;
    std::cout << "int: " << sizeof(int) <<std::endl;
    std::cout << "short: " << sizeof(short) << std::endl;
    std::cout << "float: " << sizeof(float) << std::endl;
    std::cout << "double: " << sizeof(double) << std::endl;
    std::cout << "unsigned int: " << sizeof(unsigned int) << std::endl;
    std::cout << "char: " << sizeof(char) << std::endl;
    std::cout << "signed int: " << sizeof(signed int) << std::endl;
    std::cout << "char*: " << sizeof(p) << std::endl;
    std::cout << "char array: " << sizeof(c) << std::endl;
    std::cout << "string: " << sizeof(std::string("123")) << std::endl;
    std::cout << "long: " << sizeof(long) << std::endl;
    std::cout << "long long: " << sizeof(long long) << std::endl;
    std::cout << "Class A: " << sizeof(A) << std::endl;
    std::cout << "Class B: " << sizeof(B) << std::endl;
    
    return 0;
}
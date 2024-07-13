#include <iostream>
#include <chrono>
#include <ctime>
#include <iomanip>
#include <thread>

inline char separator()
{
#ifdef _WIN32
    return '\\';
#else
    return '/';
#endif
}

const char *file_name(const char *path)
{
    const char *file = path;
    while (*path)
    {
        if (*path++ == separator())
        {
            file = path;
        }
    }
    return file;
}

void PrintTime()
{
    using namespace std;
    using namespace std::chrono;

    auto now = system_clock::now();
    auto in_time_t = system_clock::to_time_t(now);

    // get number of milliseconds for the current second
    // (remainder after division into seconds)
    auto ms = duration_cast<milliseconds>(now.time_since_epoch()) % 1000;

    cout << std::put_time(std::localtime(&in_time_t), "%T")
         << '.' << std::setfill('0') << std::setw(3) << ms.count();
}

void PrintThread()
{
    using namespace std;
    cout << " [Thread-" << this_thread::get_id() << "] ";
}
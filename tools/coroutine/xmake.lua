set_languages("c++20")
add_rules("mode.debug", "mode.release")

target("Coroutine")
    set_kind("binary")
    add_files("/AnsyncTask/*.cpp")
    add_includedirs("/AnsyncTask")
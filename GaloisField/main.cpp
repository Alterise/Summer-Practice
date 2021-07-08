#include <iostream>
#include "galois_field.hpp"

int main() {
    std::cout << galois_field(0b00100011) + galois_field(0b10010111) << std::endl;
    std::cout << (galois_field(0b11100101) * galois_field(0b00101010)) << std::endl;
    std::cout << ~galois_field(0b00101101) << std::endl;
    std::cout << "Irreducible polynomials:" << std::endl;
    auto vec = galois_field::get_all_irreducible_galois_fields();
    for (const auto &item : vec) {
        std::cout << item << std::endl;
    }
    return 0;
}
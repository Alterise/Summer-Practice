#include <iostream>
#include "galois_field.hpp"

int main() {
    std::cout << galois_field(0b00100011) + galois_field(0b10010111) << std::endl;
    std::cout << (galois_field(0b11100101) * galois_field(0b00101010)) << std::endl;
    std::cout << ~galois_field(0b00101101) << std::endl;

    std::cout << galois_field::get_all_irreducible_galois_fields() << std::endl;

    return 0;
}
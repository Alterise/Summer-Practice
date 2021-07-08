#pragma once

#include <cstdint>
#include <bitset>
#include <iostream>
#include <vector>

class galois_field {
public:
    const static uint16_t modulo = 0b100011101;
    static std::vector<galois_field> get_all_irreducible_galois_fields();

    explicit galois_field(uint16_t value = 0);
    void set(uint16_t value);
    uint16_t get() const;
    friend galois_field operator+(const galois_field& lhs_field, const galois_field& rhs_field);
    friend galois_field operator*(const galois_field& lhs_field, const galois_field& rhs_field);
    friend galois_field operator~(const galois_field& field);
    friend std::ostream& operator<<(std::ostream& out, const galois_field& field);
private:
    uint16_t _value;
};
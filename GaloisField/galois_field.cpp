#include "galois_field.hpp"

galois_field::galois_field(uint16_t value) : _value(value) {}

void galois_field::set(uint16_t value) {
    _value = value;
}

uint16_t galois_field::get() const {
    return _value;
}

galois_field operator+(const galois_field& lhs_field, const galois_field& rhs_field) {
    return galois_field(lhs_field._value ^ rhs_field._value);
}

int degree(uint16_t value) {
    std::bitset<16> bs(value);
    int i = 16;
    while (!bs[--i] && i >= 0);
    return i;
}

uint16_t mod(uint16_t value, const uint16_t modulo = galois_field::modulo) {
    int difference;
    int poly_degree;
    int modulo_degree = degree(modulo);
    while ((difference = (poly_degree = degree(value)) - modulo_degree) >= 0) {
        value ^= (modulo << difference);
    }
    return value;
}

galois_field operator*(const galois_field& lhs_field, const galois_field& rhs_field) {
    uint16_t mult_result = 0;
    std::bitset<8> lhs_bitset(lhs_field._value);
    for (int i = 7; i >= 0; --i) {
        if (lhs_bitset[i]) {
            mult_result = mult_result ^ (rhs_field._value << i);
        }
    }

    return galois_field(mod(mult_result));
}

galois_field pow(const galois_field& field, uint8_t power) {
    if (power <= 0) {
        return galois_field(1);
    } else if (power % 2) {
        return field * pow(field, power - 1);
    } else {
        return pow(field * field, power / 2);
    }
}

galois_field operator~(const galois_field& field) {
    return pow(field, 254);
}

std::vector<galois_field> galois_field::get_all_irreducible_galois_fields() {
    std::vector<galois_field> result;

    for (uint16_t i = 0b100000001; i <= 0b111111111; i += 2) {
        uint16_t j;
        for (j = 0b000010; j <= 0b11111; ++j) {
            if (mod(i, j) == 0) {
                break;
            }
        }
        if (j == 0b100000) {
            result.emplace_back(i);
        }
    }
    return result;
}

std::ostream& operator<<(std::ostream& out, const galois_field& field) {
    std::bitset<16> binary_representation(field._value);
    std::string polynomial_representation;
    for (int i = 15; i >= 0; --i) {
        if (binary_representation[i]) {
            if (!polynomial_representation.empty()) {
                polynomial_representation += " + ";
            }
            if (i > 1) {
                polynomial_representation += "x^" + std::to_string(i);
            } else if (i == 1) {
                polynomial_representation += "x";
            } else {
                polynomial_representation += "1";
            }
        }
    }
    if (polynomial_representation.empty()) {
        polynomial_representation += "0";
    }
    out << polynomial_representation;
    return out;
}

std::ostream& operator<<(std::ostream& out, const std::vector<galois_field>& vec) {
    out << "Irreducible polynomials:" << std::endl;

    for (const auto &field : vec) {
        out << field << std::endl;
    }
    return out;
}
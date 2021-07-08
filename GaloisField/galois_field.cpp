#include "galois_field.h"

galois_field::galois_field() : _value(0) = default;

void galois_field::set(uint8_t value) {
    _value = value;
}

uint8_t galois_field::get() {
    return _value;
}

void galois_field::add(galois_field rhs) {
    _value ^= rhs._value;
}

friend operator<<(ostream& stream, )
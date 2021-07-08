#pragma once

#include <cstdint>
#include <bitset>
#include <iostream>

class galois_field {
public:
    galois_field();
    void add(galois_field rhs);
    void multiply(galois_field rhs, galois_field modulo);
    void inverse(galois_field modulo);
    void set(unsigned);
    uint8_t get();
private:
    uint8_t _value;
};
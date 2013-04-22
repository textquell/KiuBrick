#! /bin/bash

mkdir m4 &&
autoreconf --install &&
./configure &&
make

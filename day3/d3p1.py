# by Kamiel de Visser

# Open file
from types import resolve_bases
from typing import ForwardRef


with (open("./day3/input.txt") as file):
    numberStrings = [ line.rstrip("\n") for line in file.readlines() ]

numberLength = len(numberStrings[0])

result = ""
for i in range (0, numberLength):
    zeroes = 0
    ones = 0
    for numberString in numberStrings:
        if (numberString[i] == "1"):
            ones += 1
        else: 
            zeroes += 1
    if (ones >= zeroes):
        result += '1'
    else :
        result += '0'

binary_dict = {'0': '1', '1': '0'}
inverse = ""

for i in result:
    inverse += binary_dict[i]

print("gamma rate:", int(result, 2))
print("epsilon rate:", int(inverse, 2))
print("Power consumption: ", int(result, 2) * int(inverse, 2))
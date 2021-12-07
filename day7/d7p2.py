# by Kamiel de Visser

from os import linesep


with (open("./day7/input.txt") as file):
    numberStrings = [line.rstrip("\n") for line in file.readlines()]
    numbers = [int(numberString) for numberString in numberStrings[0].split(",")]


min = 1000000000
for desiredPos in range(2000):
    fuel = 0
    for pos in numbers:
        absPos = abs(desiredPos - pos)
        for i in range(1, absPos + 1):
            fuel += i
    
    if (fuel < min):
        min = fuel

print(min)
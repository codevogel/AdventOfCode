# by Kamiel de Visser

from os import linesep


with (open("./day7/input.txt") as file):
    numberStrings = [line.rstrip("\n") for line in file.readlines()]
    numbers = [int(numberString) for numberString in numberStrings[0].split(",")]


min = 1000000000
for desiredPos in range(2000):
    fuel = 0
    for pos in numbers:
        fuel += abs(desiredPos - pos)
    
    if (fuel < min):
        min = fuel

print(fuel)
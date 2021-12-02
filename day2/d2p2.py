# by Kamiel de Visser

# Open file
with (open("./day2/input.txt") as file):
    # List comprehension generates tuples containing (dir, value)
    # dir is first char of instruction
    # value is a single digit integer
    instructions = [ (line[0] , int(line[len(line)-2])) for line in file.readlines() ]

# Declare starting values
aim = 0
depth = 0
horizontal = 0

# For each instruction, increment or decrement respective values
for instruction in instructions:
    dir = instruction[0]
    val = instruction[1]
    # Pick instruction
    match dir:
        case'u':
            aim -= val
        case'd':
            aim += val
        case'f':
            horizontal += val
            depth += aim * val

# Print answer
print("Horizontal * Depth at end of instructions:", horizontal * depth)
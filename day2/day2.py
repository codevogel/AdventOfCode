

with (open("./day2/input.txt") as file):
    lines = [[line[0],int(line.rstrip("\n")[len(line)-2])] for line in file.readlines()]

aim = 0
depth = 0
horizontal = 0

for instruction in lines:
    dir = instruction[0]
    val = instruction[1]
    match dir:
        case'u':
            aim -= val
        case'd':
            aim += val
        case'f':
            horizontal += val
            depth += aim * val

print("Horizontal * Depth at end of instructions:", horizontal * depth)
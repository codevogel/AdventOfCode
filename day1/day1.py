with open("./day1/input.txt") as file: lines = [int(line.rstrip('\n')) for line in file.readlines() if line.isdigit()]

larger = 0

for i in range(0, len(lines)-3):
    try:
        a = lines[i] + lines [i + 1] + lines [i + 2]
        b = lines[i + 1] + lines [i + 2] + lines [i + 3]
        if (a < b):
            larger += 1
    except:
        continue

print(lines)
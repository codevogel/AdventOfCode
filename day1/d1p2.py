# by Kamiel de Visser

# List comprehend the input file to a list of integers
with open("./day1/input.txt") as file: nums = [int(line.rstrip('\n')) for line in file.readlines()]

# Declare starting value
larger = 0

# Loop through all nums in three-measurement sliding window
for i in range(0, len(nums)-3):
    a = nums[i] + nums [i + 1] + nums [i + 2]
    b = nums[i + 1] + nums [i + 2] + nums [i + 3]

    # Increment when needed
    if (a < b):
        larger += 1

# Print answer
print("Times number has increased:", larger)
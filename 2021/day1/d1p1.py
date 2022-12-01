# by Kamiel de Visser

# List comprehend the input file to a list of integers
with open("./day1/input.txt") as file: nums = [int(line.rstrip('\n')) for line in file.readlines()]

# Declare starting value
larger = 0

# Loop through all nums in three-measurement sliding window
for i in range(1, len(nums)):
    # Increment when needed
    if (nums[i - 1]  < nums [i]):
        larger += 1

# Print answer
print("Times number has increased:", larger)
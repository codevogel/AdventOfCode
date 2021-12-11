currentDay = 12

for day in range(1, currentDay):
    with (open("./day" + str(day) +"/README.MD") as file):
        lines = file.readlines()
        if not (lines[0].startswith("# Day")):
            continue
        lines.insert(0, "It's contents are originally written by Eric Wastl, and slightly modified with my answers.\n\n\n")
        lines.insert(0, "Advent of Code is a registered trademark in the United States.\n")
        lines.insert(0, "This readme is archived from https://adventofcode.com/2021/day/" + str(day) + " and may not be commercially redistributed.\n")

    with (open("./day" + str(day) +"/README.MD", "w") as file):
        for item in lines:
            file.write(item)
    
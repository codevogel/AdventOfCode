import sys;

def updateReadmes(year, day):
    for currentDay in range (1, int(day) + 1):
        path = "C:\\workspace\\AdventOfCode\\" + year + "\\day" + str(currentDay) + '\\README.MD'
        with (open(path) as file):
            lines = file.readlines()
            if lines[0].startswith("--- Day"):
                lines.insert(0, "It's contents are originally written by Eric Wastl, and slightly modified with my answers.\n\n\n")
                lines.insert(0, "Advent of Code is a registered trademark in the United States.\n")
                lines.insert(0, "This readme is archived from https://adventofcode.com/" + year + "/day/" + str(currentDay) + " and may not be commercially redistributed.\n")
                for i in range(len(lines)):
                    puzzleAnsStart = "Your puzzle answer was "
                    if lines[i].startswith(puzzleAnsStart):
                        lines[i] = "<details><summary>Click for the correct answer using my puzzle input.</summary>Your puzzle answer was " + lines[i][len(puzzleAnsStart):] + "</details>\n\n"

                with (open(path, 'w') as file):
                    for item in lines:
                        file.write(item)
    
if __name__ == "__main__":
    year = input("Current year?\t")
    day = input("Current day?\t")
    updateReadmes(year, day)
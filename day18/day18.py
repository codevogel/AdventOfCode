import ast
from functools import reduce
import math

INT = type(1)
LIST = type([])

class Node:

    def __init__(self, input, parent = None):
        self.parent = parent
        if (type(input) == INT):
            self.value = input
        else:
            self.value = []
            for part in input:
                self.value.append(Node(part, self))

    def isLeaf(self):
        return type(self.value) == INT

    def isRoot(self):
        return self.parent == None

    def __str__(self) -> str:
        if (self.isLeaf()):
            return str(self.value)
        return str.format("[{0},{1}]", str(self.value[0]), str(self.value[1]))

    def copy(self, parent = None):
        if self.isLeaf():
            return Node(self.value, parent)
        copy = Node(0, parent)
        copy.value = [self.value[0].copy(copy), self.value[1].copy(copy)]
        return copy
    
def fillTree(input, parent = None):
    currentNode = Node(input, parent)
    if (currentNode.isLeaf()):
        return currentNode

def snailExplode(snailNum):
    firstExplodingNode = getFirstExplodingNode(snailNum)
    if (firstExplodingNode is not None):
        firstLeft = snailGetFirstLeafInDir(True, firstExplodingNode.value[0], firstExplodingNode.value[0])
        firstRight = snailGetFirstLeafInDir(False, firstExplodingNode.value[1], firstExplodingNode.value[1])
        if (firstLeft is not None):
            firstLeft.value += firstExplodingNode.value[0].value        
        if (firstRight is not None):
            firstRight.value += firstExplodingNode.value[1].value
        firstExplodingNode.value = 0
        return True
    return False
    
def snailGetFirstLeafInDir(left, current, original):
    leftRight = 0 if left else 1
    # Search up to the root
    while(not current.isRoot()):
        previous = current
        current = current.parent
        if (current.value[leftRight] is not previous):
            break
    # Reached root
    current = current.value[leftRight] # take node in searchdir
    if (current is previous): # return None if we just came from that branch
        return None

    leftRight = 1 if left else 0
    # Search down for leaf
    while(not current.isLeaf()):
        current = current.value[leftRight]
    return current

def getFirstExplodingNode(currentNode, depth = 0):
    if (currentNode.isLeaf()):
        if (depth == 5): # Depth 5 as leaf values are it's own level
            return currentNode.parent
        return None
    for child in currentNode.value:
        explodingNode = getFirstExplodingNode(child, depth + 1)
        if (explodingNode != None):
            return explodingNode
    return None

def snailSplit(current):
    if current.isLeaf():
        if current.value > 9:
            current.value = [Node(math.floor(current.value / 2), current),Node(math.ceil(current.value / 2), current)]
            return True
        return False
    for child in current.value:
        if (snailSplit(child)):
            return True
    return False

def snailAdd(a, b):
    tree = Node(0)
    tree.value = [a,b]
    a.parent = tree
    b.parent = tree

    firstRun = True
    while(True):
        if (snailReduce(tree, firstRun)):
            firstRun = True
            continue
        if (not firstRun):
            break
        firstRun = False
    return tree
            

def snailReduce(snailNum, firstRun):
    if (firstRun):
        return snailExplode(snailNum)
    else:
        return snailSplit(snailNum)

def snailMagnitude(snailNum):
    if (snailNum.isLeaf()):
        return snailNum.value
    return 3 * snailMagnitude(snailNum.value[0]) + 2 * snailMagnitude(snailNum.value[1])

def getSnailNumbers():
    snailNumbers = []
    with (open("input.txt") as file):
        for line in file.readlines():
            snailNumbers.append(Node(ast.literal_eval(line), None))
    return snailNumbers

if __name__ == "__main__":
    numberQueue = getSnailNumbers()
    # Part 1 - Add all numbers, then reduce and calculate magnitude
    sum = numberQueue.pop(0)
    while(len(numberQueue) > 0):
        sum = snailAdd(sum, numberQueue.pop(0))
    print("Magnitude of sum", snailMagnitude(sum))

    # Part 2 - Find highest magnitude resulting form adding all non commutative pairs of numbers 
    # Get all unique combos by comparing list of combos vertically and shifting bottom list left:
    # a b c d  >>>  a b c d  >>> a b c d
    # b c d a  >>>  c d a b  >>> d a b c
    snailNumbers = getSnailNumbers()
    shiftedSnailNumbers = snailNumbers.copy()
    shiftedSnailNumbers.append(shiftedSnailNumbers.pop(0)) # preshift left
    maxMagnitude = 0
    for i in range (len(snailNumbers) - 1): # shift possible combinations left n - 1 times
        for j in range(len(snailNumbers)): # try all current combinations
            magnitude = snailMagnitude(snailAdd(snailNumbers[j].copy(), shiftedSnailNumbers[j].copy()))
            if (magnitude > maxMagnitude):
                maxMagnitude = magnitude
        shiftedSnailNumbers.append(shiftedSnailNumbers.pop(0)) # shift left
    print("Max magnitude:", maxMagnitude)

        
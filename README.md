<h1 align="center">Tiny Compiler</h1>


<h3 align="center"> Fully implemented Scanner and Parser for Tiny Compiler</h3>



## Features

- A program in TINY consists of a set of functions (any number of functions and ends with a main function).

- Each function is a sequence of statements including (declaration, assignment, write, read, if, repeat, function, comment, …). 
- Each statement consists of (number, string, identifier, expression, condition, …).



## Usage/Examples

```c#
/* Sample program in Tiny language – computes factorial*/

int main()
{

int x;
read x;     /*input an integer*/
if x > 0 then     
  int fact := 1;
  repeat
      fact := fact * x;
      x := x – 1;
  until x = 0
  write fact; /*output factorial of x*/
end
return 0;
}

```


## Overview
<p align="center">
<img src="https://user-images.githubusercontent.com/91877743/209936066-da4f78f5-1354-4bac-86b8-f8fdb46a1689.PNG" width="700" height="400">
</p>

## Test Code with Syntex Error

<p align="center">
<img src="https://user-images.githubusercontent.com/91877743/209936310-d12cc873-6623-4bbb-8be2-62b9a925a639.PNG" width="700" height="400">
</p>

## Test Code without Error

<p align="center">
<img src="https://user-images.githubusercontent.com/91877743/209936434-e049481d-3b1f-445e-85ad-960173680174.PNG" width="700" height="400">
</p>

## Documentation

[Language Description ](https://docs.google.com/document/d/1rd-Iqtm3JDBO_DAZKorV52IVK1Zz7W6q/edit#)

# Team members 
- [Abdelrhman Sayed](https://github.com/Abdelrhman-Sayed70)
- [Ruqaiyah Alarabi](https://github.com/25Ruq) 
- [Nour Ayman](https://github.com/NourAyman10)
- [Ahmed Yasser](https://github.com/ahmedYasserMohamed)
- [Haneen Ibrahim](https://github.com/HaneenIbrahim2)
- [Toqa Elsayed](https://github.com/ToqaElsayedd)

# CLI Evaluation Tool

A fully-featured C# command-line application that allows teachers to create, manage, and evaluate student test results using a simple and effective text interface. This tool supports custom answer keys, pass mark evaluation, and persistent storage of student and test data.

---

## ✨ Features

- Add, view, and delete **Tests** and **Students**
- Assign individual or multiple tests to students
- Evaluate student answers against answer keys
- Automatically calculate **pass/fail** status based on custom thresholds (e.g., "60%")
- View all student test results
- Save and load test/student data to/from `.json` files

---

## 🧱 Technologies Used

- C# (.NET)
- Object-Oriented Programming (OOP)
- Interfaces (`ITestPaper`, `IStudent`)
- LINQ for concise logic (e.g., evaluation and search)
- File I/O with **JSON** (via `Newtonsoft.Json`)
- Robust menu-driven CLI structure

---

## 📁 File Overview

All functionality is included in a **single source file** for easier distribution and review:

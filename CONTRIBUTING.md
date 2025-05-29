# Contributing to This Project

## Contribution Guidelines

1. **Folder Structure and Naming Conventions**:
   - Follow the existing folder structure stated in the README and use semantic names for any files or folders.
   - Ensure assets and scripts are placed in their appropriate directories (e.g., scripts in `Assets/Scripts`, UI assets in `Assets/UI`).

2. **Code Documentation**:
   - Use **XML-style comments** (`///`) for all public methods, properties, and classes in new or modified C# scripts.
     - Each comment should describe:
       - What the function or class does.
       - The parameters (`<param>`) it takes.
       - What it returns (`<returns>`) if applicable.
       - Any side effects or exceptions, if relevant.
     - Example:
       ```csharp
       /// <summary>
       /// Moves the player to the specified position.
       /// </summary>
       /// <param name="target">The target position.</param>
       /// <returns>True if the move was successful; otherwise, false.</returns>
       public bool MovePlayer(Vector3 target) { ... }
       ```
   - If modifying existing code, update XML comments where necessary to reflect your changes.

3. **Testing Requirements**:
   - Thoroughly test all changes before submitting pull requests.
   - Verify that the project builds and runs without errors or warnings.
   - Ensure all new features function as intended, and check compatibility with existing features.

4. **Dependencies**:
   - Add new dependencies only if necessary. Ensure they are properly documented and included in the project setup instructions.

## Branching Strategy

1. **Main Branch**:
   - Contains stable production-ready code.

2. **Feature Branches**:
   - Develop new features in branches named `feature/[name]` (e.g., `feature/new-enemy`).

## Commit Guidelines

- Use clear and descriptive commit messages.
- Use the following commit prefixes:
   - `feat: [description]` for new features.
   - `fix: [description]` for bug fixes.
   - `chore: [description]` for maintenance tasks.
   - `refactor: [description]` for cleanup or improvements.
   - `docs: [description]` for markdown files.

## Pull Requests

1. **Coding Conventions**:
   - Ensure your code follows the conventions outlined in `CONVENTIONS.md`.
   - Maintain readability and adhere to modular design principles.

2. **Pull Request Description**:
   - Provide a clear and detailed explanation of the changes you made.
   - Mention the purpose of the change, relevant issue numbers (if any), and testing steps.

3. **Review Process**:
   - All pull requests will be reviewed for quality, consistency, and adherence to the guidelines before being merged.

## Additional Notes

- If you have questions about contributing or are unsure about a change, feel free to open a discussion or issue in the repository.
- Contributions of all levels are welcome, from small bug fixes to major feature implementations.

By adhering to these guidelines, you help ensure the project remains organized, efficient, and high-quality.

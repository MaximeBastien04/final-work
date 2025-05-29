# C# Coding Conventions

## Naming Conventions

1. **Variables**:  
   - Use `camelCase` for variable names.  
     - Example: `playerHealth`, `jumpForce`.

2. **Constants**:  
   - Use `UPPERCASE_SNAKE_CASE` for constant values.  
     - Example: `MAX_JUMP_HEIGHT`, `DEFAULT_SPEED`.

3. **Functions and Methods**:  
   - Use `PascalCase` for function and method names.  
     - Example: `MovePlayer()`, `CalculateScore()`.

4. **Classes**:  
   - Use `PascalCase` for class names.  
     - Example: `PlayerController`, `GameManager`.

5. **Namespaces**:  
   - Use `PascalCase` for namespace names, matching the folder structure.  
     - Example: `Game.Utils`, `Game.UI`.

## Indentation

- Use **4 spaces** for indentation.
- Avoid tabs to ensure consistent formatting across different environments.

## Line Length

- Limit lines to **100 characters** for readability.
- If a line exceeds 100 characters, break it into multiple lines using logical groupings.

## Comments

1. **Single-Line Comments**:  
   - Use `//` for inline explanations or to describe specific lines of code.

2. **XML-Style Comments**:  
   - Use triple-slash `///` comments to document public methods, classes, and properties.  
     - These comments are picked up by IntelliSense and should include:
       - A summary of the method or property.
       - Parameter descriptions using `<param>`.
       - Return value descriptions using `<returns>`, if applicable.
     - Example:
       ```csharp
       /// <summary>
       /// Moves the player to the specified position.
       /// </summary>
       /// <param name="target">The target position to move to.</param>
       /// <returns>True if movement was successful; otherwise, false.</returns>
       public bool MovePlayer(Vector3 target) { ... }
       ```

## File Structure

1. **Class Files**:  
   - Place each class in its own file. The filename must match the class name.  
     - Example: `PlayerController.cs` should contain the `PlayerController` class.

## Additional Conventions

1. **Method Length**:  
   - Keep methods short and focused on a single responsibility.
   - If a method grows too long, refactor it into smaller, reusable methods.

2. **Error Handling**:  
   - Use `try-catch` blocks around error-prone code.
   - Log exceptions using a centralized logging mechanism.

3. **Debugging**:  
   - Use `Debug.Log()` for development purposes.
   - Remove or disable debug logs in the production build to keep the console clean.


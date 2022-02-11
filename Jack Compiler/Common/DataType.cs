public enum DataType
{
  // Primitive return types.
  INTEGER,
  BOOLEAN,
  STRING,
  CHAR,

  // Special.
  VOID, // when the function does not return anything, not valid as parameter
  CLASS_REF, // When the returned type is an object of another class.
}
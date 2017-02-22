using Brain.Neuro.Errors;

namespace Brain.Neuro
{
  public static class ErrorFunction
  {
    public static IErrorFunction Square = new SquareErrorFunction();
  }
}
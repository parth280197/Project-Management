namespace Project_Management.Helpers
{
  public static class UiHelper
  {
    public static string ClassSelector(double number)
    {
      if (number <= 10)
      {
        return "progress-bar-danger";
      }
      else if (number <= 25)
      {
        return "progress-bar-warning";
      }
      else if (number <= 50)
      {
        return "progress-bar-info";
      }
      else if (number <= 75)
      {
        return "";
      }
      else if (number == 100)
      {
        return "progress-bar-success";
      }
      return "";
    }
  }
}
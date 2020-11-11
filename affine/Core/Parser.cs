using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Render;

namespace Parser
{
  public static class PointsParser
  {
    public static List<Point2D> Parse(string input)
    {
      List<Point2D> res = new List<Point2D>();
      foreach (string line in input.Split('\n'))
      {
        Match m = Regex.Match(line, @"(-*\d+[\.,]*\d*)\s+(-*\d+[\.,]*\d*)");
        Point2D point = new Point2D(float.Parse(m.Groups[1].Value), float.Parse(m.Groups[2].Value));
        res.Add(point);
      }
      return res;
    }
  }
}

CREATE FUNCTION GetGPA (@StudentID int) 
RETURNS float
AS
BEGIN
DECLARE @ReturnValue float
SELECT @ReturnValue = ROUND(SUM (StudentTotal.TotalCredits) / SUM (StudentTotal.Credits), 2)
	FROM (
		SELECT 
			CAST(Credits as float) Credits
			, CAST(SUM(Value * Credits) as float) TotalCredits
		FROM 
			Enrollment e 
			JOIN Course c ON c.CourseID = e.CourseID
			JOIN Grade g  ON e.GradeID = g.GradeID
		WHERE
			e.StudentID = @StudentID AND
			e.GradeID IS NOT NULL
		GROUP BY
			StudentID
			, Value
			, e.courseID
			, Credits
	) StudentTotal
RETURN @ReturnValue;
END
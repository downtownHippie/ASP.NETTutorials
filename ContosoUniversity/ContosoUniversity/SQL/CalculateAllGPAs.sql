-- This code is just here so I don't lose track of it
SELECT LastName, FirstName, StudentID, ROUND(SUM (StudentTotal.TotalCredits) / SUM (StudentTotal.Credits), 2) GPA
FROM (
		SELECT 
			e.StudentID
			, CAST(Credits as float) Credits
			, CAST(SUM(Value * Credits) as float) TotalCredits
		FROM 
			Enrollment e 
			JOIN Course c ON c.CourseID = e.CourseID
			JOIN Grade g  ON e.GradeID = g.GradeID
		WHERE
			--e.StudentID = 1 AND
			e.GradeID IS NOT NULL
		GROUP BY
			StudentID
			, Value
			, e.courseID
			, Credits
		) StudentTotal
		join Student s on s.ID = StudentTotal.StudentID
GROUP BY
	StudentID, s.LastName, s.FirstName
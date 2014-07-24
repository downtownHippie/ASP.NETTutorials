CREATE PROCEDURE UpdateGPA @StudentID int AS
UPDATE GPA
	SET Value = dbo.GetGPA(@StudentID)
FROM GPA
WHERE
	StudentID = @StudentID
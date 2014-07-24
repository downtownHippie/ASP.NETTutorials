CREATE TRIGGER UpdateGPAFromInsert
ON Enrollment
AFTER INSERT AS
BEGIN
	DECLARE @InsertedStudentID AS int
	SELECT @InsertedStudentID = StudentID FROM INSERTED
	EXEC UpdateGPA @InsertedStudentID
END
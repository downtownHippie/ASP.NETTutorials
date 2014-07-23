CREATE TRIGGER UpdateGPAFromUpdateDelete
ON Enrollment
AFTER UPDATE, DELETE AS
BEGIN
	DECLARE @UpdatedStudentID AS int
	SELECT @UpdatedStudentID = StudentID FROM DELETED
	EXEC MergeGPA @UpdatedStudentID
END
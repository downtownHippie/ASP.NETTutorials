CREATE TRIGGER UpdateGPAFromUpdateDelete
ON Enrollment
AFTER UPDATE, DELETE AS
DECLARE @UpdatedStudentID AS int
BEGIN
	SELECT @UpdatedStudentID = StudentID FROM DELETED
	EXEC MergeGPA @UpdatedStudentID
END
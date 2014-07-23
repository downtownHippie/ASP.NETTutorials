CREATE TRIGGER UpdateGPAFromInsert
ON Enrollment
AFTER INSERT AS
--DECLARE @InsertedGradeID AS int
--SELECT @InsertedGradeID = GradeID FROM INSERTED
--IF @InsertedGradeID IS NOT NULL
	BEGIN
		DECLARE @InsertedStudentID AS int
		SELECT @InsertedStudentID = StudentID FROM INSERTED
		EXEC MergeGPA @InsertedStudentID
	END
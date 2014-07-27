﻿CREATE TRIGGER CreateGPAFromStudentTrigger
ON Student
AFTER INSERT AS
BEGIN
	DECLARE @InsertedStudentID AS int
	SELECT @InsertedStudentID = ID FROM INSERTED
	INSERT INTO GPA
	VALUES
		(@InsertedStudentID, NULL)
END
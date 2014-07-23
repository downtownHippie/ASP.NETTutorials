﻿CREATE PROCEDURE MergeGPA @StudentID int AS
MERGE GPA AS TARGET
USING (SELECT @StudentID) as SOURCE (StudentID)
ON (TARGET.StudentID = SOURCE.StudentID)
WHEN MATCHED THEN
	UPDATE
		SET Value = (SELECT Value FROM GetGPA(@StudentID))
WHEN NOT MATCHED THEN
INSERT (StudentID, Value)
	VALUES(SOURCE.StudentID, (SELECT Value FROM GetGPA(@StudentID)));
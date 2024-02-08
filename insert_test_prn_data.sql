-- Declare variables
DECLARE @i INT = 0;
DECLARE @sentToOptions TABLE (ID INT IDENTITY(1,1), Name NVARCHAR(50));
DECLARE @prnStatusValues TABLE (ID INT IDENTITY(1,1), Status INT);

-- Populate the options
INSERT INTO @sentToOptions (Name) VALUES ('Tesco'), ('Morrisons'), ('Argos'), ('Cadbury');
INSERT INTO @prnStatusValues (Status) VALUES (3), (7), (5), (9), (8);

-- Start the loop
WHILE @i < 30
BEGIN
    -- Insert into PRN table
    INSERT INTO PRN (Reference, Note, WasteTypeId, Category, WasteSubTypeId, SentTo, Tonnes, SiteId, CompletedDate, CreatedDate)
    VALUES (CONCAT('PRN', @i + 294055), CONCAT('Note', @i), CAST(RAND()*9+1 AS INT), 
            CASE WHEN RAND() < 0.5 THEN 1 ELSE 2 END, -- Randomly assign 1 or 2 to Category
            NULL, 
            (SELECT Name FROM @sentToOptions WHERE ID = CAST(RAND()*4+1 AS INT)), 
            CAST(RAND()*80+30 AS INT), CAST(RAND()*24+1 AS INT), DATEADD(day, -@i, GETDATE()), DATEADD(day, -@i, GETDATE()));
    -- Insert into PRNHistory table
    INSERT INTO PRNHistory (PrnId, Status, Reason, Created, CreatedBy)
    VALUES (SCOPE_IDENTITY(), 
            (SELECT Status FROM @prnStatusValues WHERE ID = CAST(RAND()*5+1 AS INT)), 
            CONCAT('Reason', @i), DATEADD(day, -@i, GETDATE()), CONCAT('SQLScript', @i));
    -- Increment the counter
    SET @i = @i + 1;
END
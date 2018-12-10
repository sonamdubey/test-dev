IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateRoadTestPages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateRoadTestPages]
GO

	

CREATE PROCEDURE [dbo].[Con_InsertUpdateRoadTestPages]

@ID NUMERIC,
@RTId NUMERIC,
@PageName VARCHAR(100),
@PageNo smallint,
@Status int OUTPUT

AS
	
BEGIN
	IF @ID = -1
		BEGIN
			if EXISTS (select Priority	from Con_RoadTestPages
			where Priority=@PageNo and RTId=@RTId)
				BEGIN
					SET @Status = 0
				END
			ELSE
				BEGIN
					INSERT INTO Con_RoadTestPages (RTId,PageName,Priority) 
					VALUES (@RTId,@PageName,@PageNo)
					SET @Status = 1
				END
		END
	ELSE
		BEGIN
			if EXISTS (select Priority	from Con_RoadTestPages
			where Priority=@PageNo and RTId=@RTId and ID!=@ID)
				BEGIN
					SET @Status = 0
				END
			ELSE
				BEGIN
					Update Con_RoadTestPages 
					Set PageName = @PageName , Priority = @PageNo
					Where ID = @ID
					SET @Status = 1
				END
		END
END


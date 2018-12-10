IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_UpdateSequence]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_UpdateSequence]
GO

	
CREATE PROCEDURE [dbo].[Con_EditCms_UpdateSequence]
(
	@BasicId AS NUMERIC(18,0),
	@Id AS Numeric(18,0),
	@ToSequence AS Int,
	@LastUpdatedBy NUMERIC(18,0)
)
AS
BEGIN
	DECLARE @FromSequence AS Int
	SELECT @FromSequence = Sequence FROM Con_EditCms_Images WHERE BasicId = @BasicId AND Id = @Id
	
	IF (@ToSequence < @FromSequence) BEGIN
		UPDATE Con_EditCms_Images SET Sequence = (Sequence + 1),  LastUpdatedTime = GETDATE(), LastUpdatedBy = @LastUpdatedBy  
		WHERE BasicId = @BasicId
		AND Sequence >= @ToSequence
		AND Sequence < @FromSequence
	END ELSE BEGIN
		UPDATE Con_EditCms_Images SET Sequence = (Sequence - 1), LastUpdatedTime = GETDATE(), LastUpdatedBy = @LastUpdatedBy 
		WHERE BasicId = @BasicId AND Sequence <= @ToSequence AND Sequence > @FromSequence
	END
	
	UPDATE Con_EditCms_Images SET Sequence = @ToSequence, LastUpdatedTime = GETDATE(), LastUpdatedBy = @LastUpdatedBy 
	WHERE BasicId = @BasicId AND Id = @Id
	
END


-- drop proc Con_EditCms_UpdateSequence    
     
--EXEC Con_EditCms_UpdateSequence 71, 112, 5    
--Select @a    


--select * from Con_EditCms_Images where BasicId= 71

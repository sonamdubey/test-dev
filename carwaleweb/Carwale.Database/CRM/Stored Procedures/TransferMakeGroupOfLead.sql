IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[TransferMakeGroupOfLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[TransferMakeGroupOfLead]
GO

	




--Name of SP/Function				: CarWale.[CRM].[TransferMakeGroupOfLead]
--Applications using SP				: CRM
--Modules using the SP				: DumpVerificationDetails.cs
--Technical department				: Database
--Summary							: Update Make Group of Leads with parameter passed Group
--Author							: Dilip V. 13-Jul-2012
--Modification history				: 1.Dilip V. 16-Jul-2012

CREATE PROCEDURE [CRM].[TransferMakeGroupOfLead]
	@LeadId		VARCHAR(8000),
	@TrGroup	SMALLINT,
	@User		NVARCHAR(10)
AS

BEGIN
SET NOCOUNT ON

	--PRINT (@LeadId);

	DECLARE @CtMakeGrp	VARCHAR(250),
			@TrMakeGrp	VARCHAR(250),
			@LeadIds	VARCHAR(10), 
			@Pos		INT,
			@LeadIdList	VARCHAR(8000)
		
	SET @LeadIdList = @LeadId
	SELECT @TrMakeGrp = Name FROM CRM_ADM_FLCGroups WHERE Id = @TrGroup
	SET @LeadIdList = LTRIM(RTRIM(@LeadIdList))+ ','
	SET @Pos = CHARINDEX(',', @LeadIdList, 1)
	IF REPLACE(@LeadId, ',', '') <> ''
	BEGIN
		WHILE @Pos > 0
			BEGIN
			SET @LeadIds = LTRIM(RTRIM(LEFT(@LeadIdList, @Pos - 1)))
			IF @LeadIds <> ''
				BEGIN
					SELECT @CtMakeGrp = CAF.Name
					FROM CRM_Leads CL 
					INNER JOIN CRM_ADM_FLCGroups CAF ON CAF.Id = CL.GroupId 
					WHERE CL.id = @LeadIds
					PRINT ('Transfered from ' + @CtMakeGrp + ' to ' + @TrMakeGrp);
					PRINT (@LeadIds);
					INSERT INTO CRM_VerificationDumpLog(LeadId,UpdatedBy,UpdatedOn,Comment,LeadType) VALUES (@LeadIds,@User,GETDATE(),'Transfered from ' + @CtMakeGrp + ' to ' + @TrMakeGrp,3)
					
					
				END
				SET @LeadIdList = RIGHT(@LeadIdList, LEN(@LeadIdList) - @Pos)
				SET @Pos = CHARINDEX(',', @LeadIdList, 1)
			END
	END	
	PRINT (@TrGroup);
	PRINT (@LeadId);
	UPDATE CRM_Leads 
	SET GroupId = @TrGroup
	WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@LeadId))
	
	
END







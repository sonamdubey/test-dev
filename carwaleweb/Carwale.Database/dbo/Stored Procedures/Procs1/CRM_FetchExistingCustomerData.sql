IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchExistingCustomerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchExistingCustomerData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchExistingCustomerData] --NULL, 'pricequote1234@gmail.com', '98901100234', NULL, NULL, NULL, NULL, NULL, NULL, NULL

	@LeadType			SMALLINT = 1,
	@CustomerId			Numeric OutPut,
	@Email				VarChar(200),
	@Mobile				VarChar(15),
	@activeLeadId		Numeric output,
	@IsVerified			Bit OutPut,
	@IsFake				Bit OutPut,
	@IsActive			Bit OutPut,
	@ActiveLeadDate		VARCHAR(50) OUTPUT,
	@ActiveLeadGroupId	INT OUTPUT,
	@ActiveLeadGroupType INT OUTPUT
				
 AS
	
BEGIN
	
	DECLARE @BlockDate DATETIME
	
	IF @Mobile <> ''
		BEGIN			
			SELECT TOP 1
				@CustomerId =  CC.Id, 
				@activeLeadId = ISNULL(CL.Id, -1),
				@IsVerified = CC.IsVerified, 
				@IsFake = CC.IsFake, 
				@IsActive = CASE ISNULL(CL.Id, -1) WHEN -1 THEN 0 WHEN 0 THEN 0 ELSE 1 END,
				@ActiveLeadDate = CC.ActiveLeadDate,
				@ActiveLeadGroupId = CC.ActiveLeadGroupId,
				@ActiveLeadGroupType = CFG.GroupType,
				@BlockDate = ISNULL(CC.BlockDate, GETDATE())
			FROM CRM_Customers CC WITH(NOLOCK) 
				LEFT JOIN CRM_Leads CL WITH(NOLOCK) ON CC.ID = CL.CNS_CustId AND CL.LeadStageId <> 3 AND CL.LeadProductType = @LeadType
				LEFT JOIN CRM_ADM_FLCGroups CFG WITH(NOLOCK) ON CL.GroupId = CFG.Id
			WHERE Email = @Email OR Mobile = @Mobile
			ORDER BY CC.IsActive DESC, IsVerified DESC, CC.Id DESC
					
			IF @@ROWCOUNT = 0
				BEGIN
					SET @CustomerId = -1
					SET @activeLeadId = -1
					SET @IsVerified = 0
					SET @IsFake = 0
					SET @IsActive = 0 
					SET @ActiveLeadDate = NULL
					SET @ActiveLeadGroupId = -1
					SET @ActiveLeadGroupType = -1
				END
			ELSE IF CONVERT(DATE,@BlockDate) > CONVERT(DATE,GETDATE())
				SET @IsFake = 1
		END
	ELSE
		BEGIN
			SELECT TOP 1
				@CustomerId =  CC.Id, 
				@activeLeadId = ISNULL(CL.Id, -1),
				@IsVerified = CC.IsVerified, 
				@IsFake = CC.IsFake, 
				@IsActive = CASE ISNULL(CL.Id, -1) WHEN -1 THEN 0 WHEN 0 THEN 0 ELSE 1 END,
				@ActiveLeadDate = CC.ActiveLeadDate,
				@ActiveLeadGroupId = CC.ActiveLeadGroupId,
				@ActiveLeadGroupType = CFG.GroupType,
				@BlockDate = ISNULL(CC.BlockDate, GETDATE())
			FROM CRM_Customers CC WITH(NOLOCK) 
				LEFT JOIN CRM_Leads CL WITH(NOLOCK) ON CC.ID = CL.CNS_CustId AND CL.LeadStageId <> 3 AND CL.LeadProductType = @LeadType
				LEFT JOIN CRM_ADM_FLCGroups CFG WITH(NOLOCK) ON CL.GroupId = CFG.Id
			WHERE Email = @Email
			ORDER BY CC.IsActive DESC, IsVerified DESC, CC.Id DESC
			
			IF @@ROWCOUNT = 0
				BEGIN
					SET @CustomerId = -1
					SET @activeLeadId = -1
					SET @IsVerified = 0
					SET @IsFake = 0
					SET @IsActive = 0 
					SET @ActiveLeadDate = NULL
					SET @ActiveLeadGroupId = -1
					SET @ActiveLeadGroupType = -1
				END
			ELSE IF CONVERT(DATE,@BlockDate) > CONVERT(DATE,GETDATE())
				SET @IsFake = 1

		END
END
















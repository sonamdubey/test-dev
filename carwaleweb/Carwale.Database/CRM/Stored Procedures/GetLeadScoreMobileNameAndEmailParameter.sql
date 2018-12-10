IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetLeadScoreMobileNameAndEmailParameter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetLeadScoreMobileNameAndEmailParameter]
GO

	-- =============================================
-- Author:   Naresh Palaiya
-- Create date: 20/05/2014
-- Description:	Fetches three parameters required for lead score calculation

CREATE PROCEDURE [CRM].[GetLeadScoreMobileNameAndEmailParameter]
        @PQID bigint,
        @LeadID bigint,
	@mobiNum_flag bit =0 output,
	@name_flag  bit =0 output,
	@email_flag  bit =0 output,
	@ls_source int output
 AS

 BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Declare @lead_source varchar(30)
	SET NOCOUNT ON;
	select @mobiNum_flag=ls_mobile_flag, @name_flag=ls_name_flag, @email_flag=ls_email_flag From PQ_ClientInfo WITH (NOLOCK) where PQId = @PqID
        set @lead_source = (select CASE CRM_LeadSource.CategoryId WHEN 3 THEN LA.Organization ELSE CRM_LeadSource.SourceName END LeadSource From CRM_LeadSource 
        WITH (NOLOCK) LEFT JOIN LA_Agencies LA WITH (NOLOCK) on LA.ID = CRM_LeadSource.SourceId where  CRM_LeadSource.LeadId = @LeadID)
     
	 --check the source of the lead and return the corresponding assigned ID 
	 IF (@ls_source like '%SEO and Direct%')
	    begin 
		   set  @ls_source = 3
		end
	  Else         
        IF (@ls_source like '%CarWale Android App%')
		begin 
		    set  @ls_source = 5
		 end
		 Else IF (@ls_source like '%CarWale Mobile%')
		begin 
		    set  @ls_source = 6
		 end
		 Else IF (@ls_source like '%Research TD Request%')
		begin 
		    set  @ls_source = 7
		 end
		  Else IF (@ls_source like '%CarWale Research%')
		begin 
		    set  @ls_source = 1
		 end
		 Else IF (@ls_source like '%Inbound Call%')
		begin 
		    set  @ls_source = 8
		 end
		Else IF (@ls_source like '%CarWale%')
		begin 
		    set  @ls_source = 2
		 end
		 else
		  begin
		    set  @ls_source = 9
		  end
 END


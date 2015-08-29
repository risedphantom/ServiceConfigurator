create proc [dbo].[IISSiteAdd]
	@IISSiteName varchar(250),
	@IISSiteHost varchar(250),
	@IISSiteGroup varchar(250),
	@ID			 int out
as
begin
	insert IISSite(SiteName, Hostname, [Group])
	values (@IISSiteName, @IISSiteHost, @IISSiteGroup)

	set @ID = scope_identity()
end
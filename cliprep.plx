#! /usr/bin/perl -w
# The very first implementation of clippy, it did not contain this comment.
use strict;
use Win32::Clipboard;
my $rx = "";
my $rp = "";
if(scalar(@ARGV) > 1){
	$rx = $ARGV[0];
	$rp = $ARGV[1];
}
$rp =~ s/\\n/\n/g;
$rp =~ s/\\t/\t/g;
$rp =~ s/\\r/\r/g;
my $txt = Win32::Clipboard::GetText();
$txt =~ s/$rx/$rp/ig;
$txt =~ s/\r//g;
$txt =~ s/\n/\r\n/g;
Win32::Clipboard::Set($txt);





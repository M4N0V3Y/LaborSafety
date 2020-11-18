// Import the core angular services.
import { ControlValueAccessor } from '@angular/forms';
import { Directive, Renderer2, Inject, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { AfterViewChecked } from '@angular/core';
import { ElementRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { DOCUMENT } from '@angular/common';
import * as XLSX from 'xlsx';
import { DomSanitizer } from '@angular/platform-browser';
import { ToasterService } from 'angular2-toaster';
import { Subject } from 'rxjs';
import { BrowserSettingsService, BrowserType } from 'src/app/core/services/browser-settings.service';
// ----------------------------------------------------------------------------------- //
// ----------------------------------------------------------------------------------- //

const noop = () => {
};

let readCompressImage: (event, maxSizePerm: number, fileName: string, ext: string) => void;
let onFileChange: (event: any) => void;
let readXLS: (e: any) => any;
let readAnyway: (e: any) => string;
let observeFiles: any;
let observeFilesChange: EventEmitter<any>;
let handleChange: (event: any) => void;
let xlsFormated: boolean;
let setLoader: (show: boolean) => void;
let setIcon: (show: boolean, ext: string, nameFile: string) => void;
let onRenderFinish: EventEmitter<boolean>;
let convertArrayToSring: (e: any) => string;
let finishLoaderJs: (ext, fileName) => void;

let elementRef: ElementRef;
let elementInsideIcone: ElementRef;
let elementInsideRef: ElementRef;
let elementInsideFile: ElementRef;
let elementInsideSpinner: ElementRef;
let elementInsideLabel: ElementRef;

let iconImg: string;
let fileNameImg: string;

let fileTypes: string;

let maxSizePermitObj: number;

let toasterService: ToasterService;

const imageExt = {
	icon: 'fa fa-file-image-o',
	ext: ['jpg', 'jpeg', 'gif', 'png']
};
const pdfExt = {
	icon: 'fa fa-file-pdf-o',
	ext: ['pdf']
};
const textExt = {
	icon: 'fa fa-file-word-o',
	ext: ['txt', 'doc', 'docx', 'odt']
};
const videoExt = {
	icon: 'fa fa-file-video-o',
	ext: ['avi', 'mov', 'mpg', 'mpeg', 'mkv', 'mp4', 'wmv']
};
const audioExt = {
	icon: 'fa fa-file-audio-o',
	ext: ['mp3', 'wma', 'acc']
};
const spreadSheetExt = {
	icon: 'fa fa-file-excel-o',
	ext: ['xls', 'xlsx', 'xlsm']
};
const anywayExt = {
	icon: 'fa fa-file-o',
	ext: []
};

function returnIconFromExt(ext: string) {
	if (imageExt.ext.filter(x => x === ext.trim().toLowerCase()).length > 0) {
		return imageExt.icon;
	} else if (pdfExt.ext.filter(x => x === ext.trim().toLowerCase()).length > 0) {
		return pdfExt.icon;
	} else if (textExt.ext.filter(x => x === ext.trim().toLowerCase()).length > 0) {
		return textExt.icon;
	} else if (videoExt.ext.filter(x => x === ext.trim().toLowerCase()).length > 0) {
		return videoExt.icon;
	} else if (audioExt.ext.filter(x => x === ext.trim().toLowerCase()).length > 0) {
		return audioExt.icon;
	} else if (spreadSheetExt.ext.filter(x => x === ext.trim().toLowerCase()).length > 0) {
		return spreadSheetExt.icon;
	} else {
		return anywayExt.icon;
	}
}

@Directive({
	selector: '[observeFiles]',
	host: {
		'(blur)': 'onTouchedCallback()',
		'(change)': 'handleChangeDrag($event)',
		'(drag)': 'handleChangeDrag($event)',
		'(dragstart)': 'handleChangeDrag($event)',
		'(dragend)': 'handleChangeDrag($event)',
		'(dragover)': 'handleChangeDrag($event)',
		'(dragenter)': 'handleChangeDrag($event)',
		'(dragleave)': 'handleChangeDrag($event)',
		'(drop)': 'handleChangeDrag($event)'
	},

	// By overriding the NG_VALUE_ACCESSOR dependency-injection token at this level of
	// the component tree / hierarchical injectors, we are effectively replacing the
	// DefaultValueAccessor for this Input Element context. As such, when Angular looks
	// for a ControlValueAccessor implementation in the local dependency-injection
	// container, it will only find this one - the one that looks at the "FileList"
	// property, not the "value" property.
	providers: [{
		provide: NG_VALUE_ACCESSOR,
		useExisting: FileInputValueAccessorDirective,
		multi: true
		// NOTE: I _believe_ that because I am using Ahead-of-Time (AoT) compiling in
		// this demo, I don't need to use the forwardRef() wrapper to reference the
		// Class that hasn't been defined yet.
	}]
})
export class FileInputValueAccessorDirective implements ControlValueAccessor, OnInit, AfterViewChecked {

	private elementRef: ElementRef;
	private onChangeCallback: () => any;
	private onTouchedCallback: () => any;
	private elementInsideRef: ElementRef;
	private elementInsideIcone: ElementRef;
	private elementInsideLabel: ElementRef;
	private elementInsideFile: ElementRef;
	private elementInsideSpinner: ElementRef;
	private iconImg: string = anywayExt.icon;
	private fileNameImg = '';

	observeFiles: any;
	@Input('observeFiles') set setObserveValue(value) {
		this.observeFiles = value;
	}
	@Output() observeFilesChange: EventEmitter<any> = new EventEmitter<any>();

	@Input() xlsFormated: boolean;

	@Input() fileTypes: string;

	@Output() onRenderFinish: EventEmitter<boolean> = new EventEmitter<boolean>();

	@Input() clear: Subject<boolean> = new Subject<boolean>();

	@Input() maxSizePermit: number;
	// I initialize the file-input value accessor service.
	constructor(
		elementRef: ElementRef,
		private renderer: Renderer2,
		@Inject(DOCUMENT) private document,
		private sanitizer: DomSanitizer,
		public toasterServiceObj: ToasterService,
		private browserSettingsService: BrowserSettingsService) {

		this.elementRef = elementRef;
		// CAUTION: These will be called by Angular when rendering the View.
		this.onChangeCallback = noop;
		this.onTouchedCallback = noop;
	}

	ngOnInit(): void {

		/* Initialize Global Variables */
		onFileChange = this.onFileChange;
		readXLS = this.readXLS;
		readAnyway = this.readAnyway;
		observeFiles = this.observeFiles;
		observeFilesChange = this.observeFilesChange;
		handleChange = this.handleChange;
		xlsFormated = this.xlsFormated;
		onRenderFinish = this.onRenderFinish;
		convertArrayToSring = this.convertArrayToSring;
		readCompressImage = this.readCompressImage;

		iconImg = this.iconImg;
		fileNameImg = this.fileNameImg;

		setIcon = this.setIcon;
		setLoader = this.setLoader;

		maxSizePermitObj = this.maxSizePermit;
		toasterService = this.toasterServiceObj;
		finishLoaderJs = this.finishLoader;

		if (this.fileTypes) {
			this.fileTypes = this.fileTypes.trim().toLowerCase();
		}

		fileTypes = this.fileTypes;

		/* End Initialize Global Variables */
		if (!this.elementRef.nativeElement.id || this.elementRef.nativeElement.id === '') {
			this.elementRef.nativeElement.id = 'file-upload-observer';
		}

		const elementDiv = document.createElement('div');
		this.elementInsideRef = new ElementRef(elementDiv);

		const elementDivSpinner = document.createElement('div');
		elementDivSpinner.className = 'overlay';
		elementDivSpinner.innerHTML = '<div class="lds-ring"><div></div><div></div><div></div><div></div></div>';
		this.elementInsideSpinner = new ElementRef(elementDivSpinner);
		this.elementInsideSpinner.nativeElement.style.display = 'none';

		const labelIcone = document.createElement('label');
		labelIcone.id = 'labelIconeInput';
		labelIcone.className = 'custom-file-upload';
		labelIcone.setAttribute('for', this.elementRef.nativeElement.id);
		this.elementInsideLabel = new ElementRef(labelIcone);

		const labelIconeInter = document.createElement('div');
		labelIconeInter.id = 'labelIconeInter';
		labelIconeInter.innerHTML = '<i class="fa fa-cloud-upload content-input"></i>';
		this.elementInsideIcone = new ElementRef(labelIconeInter);

		this.renderer.appendChild(this.elementInsideLabel.nativeElement, this.elementInsideIcone.nativeElement);
		const labelIconeText = document.createElement('label');
		labelIconeText.style.fontSize = '14px';
		labelIconeText.id = 'labelIconeInterText';
		if (this.browserSettingsService.GetBrowser() !== BrowserType.IE) {
			labelIconeText.textContent = 'Click ou arraste para inserir o arquivo.';
		} else {
			labelIconeText.textContent = 'Click para inserir o arquivo.';
		}

		this.renderer.appendChild(this.elementInsideIcone.nativeElement, labelIconeText);

		const elementDivFile = document.createElement('div');
		elementDivFile.id = 'divIconeFile';
		elementDivFile.className = 'content-input-render';
		this.elementInsideFile = new ElementRef(elementDivFile);
		this.elementInsideFile.nativeElement.style.display = 'none';

		this.renderer.appendChild(this.elementInsideLabel.nativeElement, this.elementInsideFile.nativeElement);
		this.renderer.appendChild(this.elementInsideLabel.nativeElement, this.elementInsideSpinner.nativeElement);

		this.renderer.appendChild(this.elementInsideRef.nativeElement, this.elementInsideLabel.nativeElement);

		this.renderer.insertBefore(this.elementRef.nativeElement.parentElement, this.elementInsideRef.nativeElement, this.elementRef.nativeElement);

		this.renderer.insertBefore(this.elementInsideLabel.nativeElement, this.elementRef.nativeElement, this.elementInsideLabel.nativeElement.childNodes[0]);

		this.elementRef.nativeElement.style.display = 'block';
		this.elementRef.nativeElement.style.minWidth = `${this.elementInsideLabel.nativeElement.offsetWidth}px`;
		this.elementRef.nativeElement.style.minHeight = '130px';
		this.elementRef.nativeElement.style.position = 'absolute';
		this.elementRef.nativeElement.style.opacity = 0;
		this.elementRef.nativeElement.style.cursor = 'pointer';
		this.elementRef.nativeElement.style.zIndex = 2000;

		this.elementRef.nativeElement.onchange = (e) => {
			try {
				handleChange(e);
			}
			catch (e) {
				toasterService.pop('error', '', e);
				setLoader(false);

				observeFiles = undefined;
			}
		};

		elementRef = this.elementRef;
		elementInsideRef = this.elementInsideRef;
		elementInsideFile = this.elementInsideFile;
		elementInsideSpinner = this.elementInsideSpinner;
		elementInsideIcone = this.elementInsideIcone;
		elementInsideLabel = this.elementInsideLabel;

		this.clear.subscribe(x => {
			if (x) {
				this.reset();
			}
		});
	}

	ngAfterViewChecked(): void {
		this.elementRef.nativeElement.style.minWidth = `${this.elementInsideLabel.nativeElement.offsetWidth}px`;
	}

	public handleChangeDrag(event: any): void {

		switch (event.type) {
			case 'dragenter':
			case 'dragover':
				this.renderer.addClass(this.elementInsideLabel.nativeElement, 'is-dragover');
				break;
			case 'dragleave':
			case 'dragend':
			case 'drop':
			case 'change':
				this.renderer.removeClass(this.elementInsideLabel.nativeElement, 'is-dragover');
				break;
		}
	}

	public handleChange(event: any): void {
		event.preventDefault();
		event.stopPropagation();
		if (event.dataTransfer) {
			event.dataTransfer.dropEffect = 'copy';
		}

		if (event.target.files) {
			if (event.target.files.length === 1) {
				onFileChange(event);
			}
		}
	}

	public setLoader(show: boolean): void {
		elementInsideIcone.nativeElement.style.display = show ? 'none' : 'block';
		elementInsideFile.nativeElement.style.display = show ? 'none' : 'block';
		elementInsideSpinner.nativeElement.style.display = show ? 'block' : 'none';
	}

	public reset() {
		this.observeFiles = '';
		this.elementRef.nativeElement.value = '';
		elementInsideFile.nativeElement.innerHTML = '';

		elementInsideIcone.nativeElement.style.display = 'block';
		elementInsideSpinner.nativeElement.style.display = 'none';
		elementInsideFile.nativeElement.style.display = 'none';
	}

	public setIcon(show: boolean, ext: string, nameFile: string): void {
		iconImg = returnIconFromExt(ext);
		fileNameImg = nameFile;

		elementInsideIcone.nativeElement.style.display = show ? 'none' : 'block';
		elementInsideSpinner.nativeElement.style.display = show ? 'none' : 'block';
		elementInsideFile.nativeElement.style.display = show ? 'block' : 'none';

		elementInsideFile.nativeElement.innerHTML = `<i class="${iconImg}"></i><br/>${fileNameImg}`;
	}

	private onFileChange(event: any): void {
		const file = event.target.files[0];
		const fileNameBreak = file.name.split('.');
		const ext = fileNameBreak[fileNameBreak.length - 1].trim().toLowerCase();

		const target: DataTransfer = (event.target) as DataTransfer;

		if (fileTypes && fileTypes !== '') {
			const extsValid = fileTypes.split(',');
			const existing = extsValid.filter(x => x.trim() === ext);
			if (!existing || existing.length === 0) {
				if (target && target.files && target.files.length > 0) {
					event.target.value = '';
				}
				toasterService.pop('error', '', 'Formato inválido para upload.');
				return;
			}
		}

		setLoader(true);

		/* wire up file reader */
		if (target.files.length !== 1) {
			event.target.value = '';
			toasterService.pop('error', '', 'Não pode realizar upload de múltiplos arquivos.');
			return;
		}

		if ((ext.indexOf('jpg') >= 0 || ext.indexOf('png') >= 0
			|| ext.indexOf('gif') >= 0) && (!maxSizePermitObj || maxSizePermitObj <= 0)) {
			toasterService.pop('error', '', 'Tamanho máximo de imagem não definido. Favor informar o (input) maxSizePermit para imagens.');
			return;
		}

		const reader: FileReader = new FileReader();
		reader.onload = (e: any) => {
			if ((ext.indexOf('jpg') >= 0 || ext.indexOf('png') >= 0
				|| ext.indexOf('gif') >= 0) && !xlsFormated) {
				readCompressImage(e, maxSizePermitObj, fileNameBreak[0], ext);
			} else if (ext.indexOf('xls') >= 0 && xlsFormated) {
				observeFiles = readXLS(e);
				finishLoaderJs(ext, fileNameBreak[0]);
			} else {
				observeFiles = readAnyway(e);
				finishLoaderJs(ext, fileNameBreak[0]);
			}
		};

		if (ext.indexOf('jpg') >= 0 || ext.indexOf('png') >= 0 || ext.indexOf('gif') >= 0) {
			reader.readAsDataURL(target.files[0]);
		} else {
			reader.readAsBinaryString(file);
		}
	}

	finishLoader(fileName, ext): void {
		setLoader(false);
		setIcon(true, ext, fileName);

		observeFilesChange.emit(observeFiles);
		onRenderFinish.emit(true);
	}

	convertArrayToSring(e): string {
		let binary = '';
		const bytes = new Uint8Array(e.target.result);
		const length = bytes.byteLength;

		for (let i = 0; i < length; i++) {
			binary += String.fromCharCode(bytes[i]);
		}

		return binary;
	}

	readCompressImage(e, maxSizePerm: number, fileName: string, ext: string): void {
		let width = 0;
		let height = 0;
		const img = new Image();
		img.src = e.target.result;
		img.onload = () => {
			const elem = document.createElement('canvas');

			width = img.width;
			height = img.height;

			if (width > maxSizePerm) {
				const percenSub = (100 * (width - maxSizePerm)) / width;

				width = maxSizePerm;
				height = height - ((height * percenSub) / 100);
			}

			if (height > maxSizePerm) {
				const percenSub = (100 * (height - maxSizePerm)) / height;

				width = width - ((width * percenSub) / 100);
				height = maxSizePerm;
			}

			elem.width = width;
			elem.height = height;

			const ctx = elem.getContext('2d');
			ctx.drawImage(img, 0, 0, width, height);
			ctx.canvas.toBlob((blob) => {
				const file = new File([blob], fileName, {
					type: 'image/jpeg',
					lastModified: Date.now()
				});

				const reader: FileReader = new FileReader();
				reader.onload = (r: any) => {
					observeFiles = readAnyway(r);
					finishLoaderJs(ext, fileName);
				};

				reader.readAsBinaryString(file);
			}, 'image/jpeg', 1);
		}
	}

	readXLS(e: any): any {
		/* read workbook */

		const bstr: string = e.target.result;
		const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });

		/* grab first sheet */
		const wsname: string = wb.SheetNames[0];
		const ws: XLSX.WorkSheet = wb.Sheets[wsname];
		/* save data */
		return XLSX.utils.sheet_to_json(ws, { header: 1, blankrows: false, defval: '' });
	}

	readAnyway(e: any): string {
		const bstr: string = e.target.result;
		return bstr;
	}

	public registerOnChange(callback: () => any): void {
		this.onChangeCallback = callback;
	}

	public registerOnTouched(callback: () => any): void {
		this.onTouchedCallback = callback;
	}

	public setDisabledState(isDisabled: boolean): void {
		this.elementRef.nativeElement.disabled = isDisabled;
	}

	public writeValue(value: any): void {
		if (value instanceof FileList) {
			this.elementRef.nativeElement.files = value;
		} else if (Array.isArray(value) && !value.length) {
			this.elementRef.nativeElement.files = null;
		} else if (value === null) {
			this.elementRef.nativeElement.files = null;
		}
	}
}
import { describe, it, expect, beforeEach, afterEach } from 'vitest'
import { shallowMount } from '@vue/test-utils'
import Pagination from '~/components/Pagination.vue'

describe('Pagination.vue', () =>
{
    let wrapper

    beforeEach(() => wrapper = null)

    afterEach(() =>
    {
        if (wrapper) wrapper.unmount()
    })

    it('renders total pages and highlights current page', () =>
    {
        wrapper = shallowMount(Pagination, {
            props: {
                totalPages: 3,
                currentPage: 2,
            }
        })

        const pageButtons = wrapper.findAll('button')
            .filter(btn => !['<', '>'].includes(btn.text()))
        expect(pageButtons.length).toBe(3)
        expect(pageButtons[1].classes()).toContain('btn-active')
    })

    it('disables "<" button on first page', () =>
    {
        wrapper = shallowMount(Pagination, {
            props: {
                totalPages: 3,
                currentPage: 1,
            }
        })

        const prevButton = wrapper.find('button')
        expect(prevButton.attributes('disabled')).toBeDefined()
    })

    it('disables ">" button on last page', () =>
    {
        wrapper = shallowMount(Pagination, {
            props: {
                totalPages: 3,
                currentPage: 3,
            }
        })

        const nextButton = wrapper.findAll('button').at(-1)
        expect(nextButton.attributes('disabled')).toBeDefined()
    })

    it('emits loadPage with correct number when a page is clicked', async () =>
    {
        wrapper = shallowMount(Pagination, {
            props: {
                totalPages: 3,
                currentPage: 2,
            }
        })

        const pageButtons = wrapper.findAll('button')
            .filter(btn => !['<', '>'].includes(btn.text()))
        await pageButtons[0].trigger('click')
        expect(wrapper.emitted().loadPage).toBeTruthy()
        expect(wrapper.emitted().loadPage[0]).toEqual([1])
    })

    it('emits loadPage with currentPage - 1 when "<" is clicked', async () =>
    {
        wrapper = shallowMount(Pagination, {
            props: {
                totalPages: 3,
                currentPage: 2,
            }
        })

        const prevButton = wrapper.find('button')
        await prevButton.trigger('click')
        expect(wrapper.emitted().loadPage).toBeTruthy()
        expect(wrapper.emitted().loadPage[0]).toEqual([1])
    })

    it('emits loadPage with currentPage + 1 when ">" is clicked', async () =>
    {
        wrapper = shallowMount(Pagination, {
            props: {
                totalPages: 3,
                currentPage: 2,
            }
        })

        const nextButton = wrapper.findAll('button').at(-2)
        await nextButton.trigger('click')
        expect(wrapper.emitted().loadPage).toBeTruthy()
        expect(wrapper.emitted().loadPage[0]).toEqual([3])
    })
})